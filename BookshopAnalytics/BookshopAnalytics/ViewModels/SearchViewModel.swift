//
//  PublisherViewModel.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 13/07/2025.
//

import Foundation

enum SearchEntity: String, CaseIterable {
    case publisher = "publishers"
    case author = "authors"
    case genre = "genres"
    case language = "languages"
}

/// A view model responsible for managing publishers
///
/// This class is an `@Observable` object, making it suitable for use in SwiftUI views
/// to automatically update the UI when item data or loading states change.
/// It operates on the `@MainActor` to ensure UI updates happen on the main thread.
@MainActor
@Observable
class SearchViewModel: @preconcurrency SearchViewModelProtocol {
    
    /// List of publishers loaded from the API
    var briefEntities: [BriefEntity] = []
    
    /// Status code of the response
    var statusCode: Int?
    
    /// Loads the data for all publishers from the API.
    func fetchBriefEntities(searchEntity: SearchEntity, searchTerm: String) async {

        guard let url = URL(string: "http://localhost:5084/api/\(searchEntity.rawValue)/search?searchTerm=\(searchTerm)") else {
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
    
    private func fetchBriefEntityById(searchEntity: SearchEntity, id: Int) async -> BriefEntity? {
        
        guard let url = URL(string: "http://localhost:5084/api/\(searchEntity.rawValue)/\(id)") else {
            print("Invalid URL for log in.")
            return nil
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
                return nil
            }
            
            statusCode = httpResponse.statusCode
            
            if httpResponse.statusCode != 200 {
                print("\(httpResponse.statusCode) error occured")
                return nil
            } else {
                if searchEntity == .author {
                    let jsonObject = try JSONSerialization.jsonObject(with: data, options: [])

                    if let dictionary = jsonObject as? [String: Any],
                       let idValue = dictionary["id"],
                       let id = (idValue as? Int) ?? Int("\(idValue)") {
                        
                        let pseudonym = dictionary["pseudonym"] as? String
                        let name: String
                        
                        if let pseudonym = pseudonym, !pseudonym.isEmpty {
                            name = pseudonym
                        } else {
                            let firstName = dictionary["name"] as? String ?? ""
                            let surname = dictionary["surname"] as? String ?? ""
                            name = "\(firstName) \(surname)"
                        }
                        
                        return BriefEntity(id: id, name: name)
                    }
                }
                else {
                    return try JSONDecoder().decode(BriefEntity.self, from: data)
                }
                return nil
            }
            
        } catch {
            print("Failed to load entity: \(error.localizedDescription)")
            return nil
        }
    }
    
    func fetchSelectedEntities(searchEntity: SearchEntity, ids: [Int]) async -> [BriefEntity] {
        var selectedEntities: [BriefEntity] = []
        for id in ids {
            if let entity = await fetchBriefEntityById(searchEntity: searchEntity, id: id) {
                selectedEntities.append(entity)
            }
        }
        return selectedEntities
    }
    
}
