//
//  OrderViewModel.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation

/// A view model responsible for fetching and managing the details of a single order.
///
/// This class is an `@Observable` object, making it suitable for use in SwiftUI views
/// to automatically update the UI when order data or loading states change.
/// It operates on the `@MainActor` to ensure UI updates happen on the main thread.
@MainActor
@Observable
class OrderViewModel {
    /// The detailed order object, loaded from the API.
    var order: Order?
    /// A boolean indicating if data is currently being loaded.
    var isLoading = false
    /// A string holding any error message that occurs during data fetching.
    var errorMessage: String?

    /// Loads the details for a specific order from the API.
    ///
    /// - Parameter orderId: The unique identifier of the order to be loaded.
    func loadOrder(orderId: Int) async {
        isLoading = true
        errorMessage = nil

        guard let url = URL(string: "http://localhost:5084/api/orders/\(orderId)") else {
            errorMessage = "Invalid URL"
            isLoading = false
            return
        }
        do {
            let (data, _) = try await URLSession.shared.data(from: url)

            let decoder = JSONDecoder()
            let formatter = DateFormatter()
            formatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss"
            decoder.dateDecodingStrategy = .formatted(formatter)
            order = try decoder.decode(Order.self, from: data)
        } catch {
            errorMessage = "Failed to load order: \(error.localizedDescription)"
            order = nil
        }

        isLoading = false
    }
}
