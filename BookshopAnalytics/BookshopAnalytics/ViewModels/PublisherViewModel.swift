//
//  PublisherViewModel.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 13/07/2025.
//

import Foundation

/// A view model responsible for managing publishers
///
/// This class is an `@Observable` object, making it suitable for use in SwiftUI views
/// to automatically update the UI when item data or loading states change.
/// It operates on the `@MainActor` to ensure UI updates happen on the main thread.
@MainActor
@Observable
class PublisherViewModel: @preconcurrency SearchViewModelProtocol {
    /// List of publishers loaded from the API
    var briefEntities: [BriefEntity] = []
    
    /// Status code of the response
    var statusCode: Int?
    
    /// Loads the data for all publishers from the API.
    func fetchBriefEntities(searchTerm: String) async {

        guard let url = URL(string: "http://localhost:5084/api/publishers/search?searchTerm=\(searchTerm)") else {
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
                briefEntities = try JSONDecoder().decode([BriefEntity].self, from: data)
            }
            
        } catch {
            print("Failed to load publisher list: \(error.localizedDescription)")
            briefEntities = []
        }
    }
}
