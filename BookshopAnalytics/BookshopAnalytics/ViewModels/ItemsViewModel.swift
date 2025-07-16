//
//  OrderViewModel.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation

/// A view model responsible for managing items
///
/// This class is an `@Observable` object, making it suitable for use in SwiftUI views
/// to automatically update the UI when item data or loading states change.
/// It operates on the `@MainActor` to ensure UI updates happen on the main thread.
@MainActor
@Observable
class ItemsViewModel {
    
    /// Lisy of items loaded from the API
    var items: [SimpleItem] = []
    
    /// Selected Item
    var selectedItem: Item? = nil
    
    /// Status code of the response
    var statusCode: Int?
    
    /// Loads the data for all items from the API.
    func fetchItems() async {

        guard let url = URL(string: "http://localhost:5084/api/items") else {
            print("Invalid URL for log in.")
            return
        }
        
        // Configure the URL request for a GET operation.
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
                items = try JSONDecoder().decode([SimpleItem].self, from: data)
            }
            
        } catch {
            print("Failed to load employee: \(error.localizedDescription)")
            items = []
        }
    }
    
    /// Calls delete item API request
    func deleteItem(_ id: Int) async {

        guard let url = URL(string: "http://localhost:5084/api/items/\(id)") else {
            print("Invalid URL for log in.")
            return
        }
        
        // Configure the URL request for a DELETE operation.
        var request = URLRequest(url: url)
        request.httpMethod = "DELETE"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.setValue("Bearer \(KeychainHelper.load(forKey: "access_token") ?? "")", forHTTPHeaderField: "Authorization")

        do {
            let (_, response) = try await URLSession.shared.data(for: request)
            
            guard let httpResponse = response as? HTTPURLResponse else {
                print("Response was lost or has invalid format!")
                return
            }
            
            statusCode = httpResponse.statusCode
            
            if httpResponse.statusCode != 204 {
                print("\(httpResponse.statusCode) error occured")
            } else {
                print("Deleted successfully!")
                items.removeAll(where: { $0.id == id })
            }
            
        } catch {
            print("Failed to load employee: \(error.localizedDescription)")
        }
    }
    
    /// Loads data for a specific item from the API
    func fetchItem(_ id: Int) async {

        guard let url = URL(string: "http://localhost:5084/api/items/\(id)") else {
            print("Invalid URL for log in.")
            return
        }
        
        // Configure the URL request for a GET operation.
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
                selectedItem = try decoder.decode(Item.self, from: data)
            }
            
        } catch {
            print("Failed to load employee: \(error.localizedDescription)")
        }
    }
    
}
