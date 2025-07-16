//
//  OrderViewModel.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation

/// A view model responsible for fetching and managing a single item's details.
///
/// This class is an `@Observable` object, making it suitable for use in SwiftUI views
/// to automatically update the UI when item data or loading states change.
/// It operates on the `@MainActor` to ensure UI updates happen on the main thread.
@MainActor
@Observable
class ItemViewModel {
    /// The detailed item object, loaded from the API.
    var item: Item?
    /// Bool values that tells whether customer ever received a specific item, loaded from the API.
    var customerReceivedItem: Bool?
    /// A boolean indicating if data is currently being loaded.
    var isLoading = false
    /// A string holding any error message that occurs during data fetching.
    var errorMessage: String?

    /// Loads the details for a specific item from the API.
    ///
    /// - Parameter itemId: The unique identifier of the item to be loaded.
    func loadItem(itemId: Int) async {
        isLoading = true
        errorMessage = nil
        defer { isLoading = false }

        guard let url = URL(string: "http://localhost:5084/api/items/\(itemId)") else {
            errorMessage = "Invalid URL"
            return
        }

        do {
            let (data, response) = try await URLSession.shared.data(from: url)
            guard let httpResponse = response as? HTTPURLResponse,
                  httpResponse.statusCode == 200 else {
                errorMessage = "Server returned unexpected response."
                return
            }
            let decoder = JSONDecoder()
            let fmt = DateFormatter()
            fmt.dateFormat = "yyyy-MM-dd'T'HH:mm:ss"
            decoder.dateDecodingStrategy = .formatted(fmt)
            item = try decoder.decode(Item.self, from: data)
        } catch {
            // Handle NSURLSession cancellation
            if let urlErr = error as? URLError, urlErr.code == .cancelled {
                // benign cancellation: do not reset item or show error
            }
            else if Task.isCancelled {
                // Swift concurrency cancellation: ignore
            }
            else {
                errorMessage = "Failed to load item: \(error.localizedDescription)"
                item = nil
            }
        }
    }

    /// Checks if a specific customer has received a particular item.
    ///
    /// - Parameters:
    ///   - customerId: The unique identifier of the customer.
    ///   - itemId: The unique identifier of the item.
    func hasReceivedItem(customerId: Int, itemId: Int) async {
        customerReceivedItem = nil

        guard let url = URL(string: "http://localhost:5084/api/users/customers/\(customerId)/received-item/\(itemId)") else {
            return
        }
        var request = URLRequest(url: url)
        request.httpMethod = "GET"

        do {
            let (data, response) = try await URLSession.shared.data(for: request)
            guard let httpResponse = response as? HTTPURLResponse,
                  httpResponse.statusCode == 200 else {
                return
            }
            customerReceivedItem = try JSONDecoder().decode(Bool.self, from: data)
        } catch {
            print("Error fetching hasReceivedItem:", error)
        }
    }
}
