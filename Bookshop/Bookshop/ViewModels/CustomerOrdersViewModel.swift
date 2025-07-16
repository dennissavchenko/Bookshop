//
//  CustomerOrdersViewModel.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation
import SwiftUI

/// A view model responsible for fetching and managing a customer's orders.
///
/// This class is an `@Observable` object, making it suitable for use in SwiftUI views
/// to automatically update the UI when `orders` changes. It operates on the `@MainActor`
/// to ensure UI updates happen on the main thread.
@MainActor
@Observable
class CustomerOrdersViewModel {
    /// An array containing a simplified list of the customer's orders.
    var orders: [SimpleOrder] = []

    /// Fetches the orders for a specific customer from the API.
    ///
    /// - Parameter customerId: The unique identifier of the customer whose orders are to be fetched.
    func fetchOrders(for customerId: Int) async {
        // Construct the URL for the API endpoint.
        guard let url = URL(string: "http://localhost:5084/api/users/customers/\(customerId)/orders") else {
            print("Invalid URL for fetching orders.")
            return
        }

        do {
            // Perform the network request to fetch order data.
            let (data, _) = try await URLSession.shared.data(from: url)

            // Configure JSONDecoder to handle the specific date format from the API.
            let decoder = JSONDecoder()
            let formatter = DateFormatter()
            formatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss" // Matches the API's date format
            decoder.dateDecodingStrategy = .formatted(formatter)

            // Decode the fetched data into an array of SimpleOrder objects.
            orders = try decoder.decode([SimpleOrder].self, from: data)
        } catch {
            // Print any errors encountered during the fetch or decode process.
            print("Failed to fetch orders:", error)
        }
    }
}
