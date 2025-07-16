//
//  OrderViewModel.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation

/// A view model responsible for managing orders
///
/// This class is an `@Observable` object, making it suitable for use in SwiftUI views
/// to automatically update the UI when item data or loading states change.
/// It operates on the `@MainActor` to ensure UI updates happen on the main thread.
@MainActor
@Observable
class OrdersViewModel {
    
    /// Lisy of items loaded from the API
    var orders: [SimpleOrder] = []
    
    /// Status code of the response
    var statusCode: Int?
    
    /// Loads the data for a specific employee from the API.
    func fetchOrders(orderStatus: OrderStatus) async {
        
        guard let url = URL(string: "http://localhost:5084/api/orders/status/\(orderStatus.intValue)") else {
            print("Invalid URL for log in.")
            return
        }
        
        // Configure the URL request for a POST operation.
        var request = URLRequest(url: url)
        request.httpMethod = "GET"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.setValue("Bearer \(KeychainHelper.load(forKey: "access_token") ?? "")", forHTTPHeaderField: "Authorization")
        
        do {
            let (data, response) = try await URLSession.shared.data(for: request)
            
            guard let httpResponse = response as? HTTPURLResponse else {
                print("Response was lost or has invalid format!")
                return
            }
            
            statusCode = httpResponse.statusCode
            
            if httpResponse.statusCode != 200 {
                print("\(httpResponse.statusCode) error occured")
            } else {
                let decoder = JSONDecoder()
                let formatter = DateFormatter()
                formatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss"
                decoder.dateDecodingStrategy = .formatted(formatter)
                orders = try decoder.decode([SimpleOrder].self, from: data)
            }
            
        } catch {
            print("Failed to load orders: \(error.localizedDescription)")
            orders = []
        }
    }
    
}
