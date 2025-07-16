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
class LanguageViewModel: @preconcurrency SearchViewModelProtocol {
    /// List of publishers loaded from the API
    var briefEntities: [BriefEntity] = []
    
    /// Status code of the response
    var statusCode: Int?
    
    /// Loads the data for all publishers from the API.
    func fetchBriefEntities(searchTerm: String) async {
        guard let url = Bundle.main.url(forResource: "Languages", withExtension: "json"),
              let data = try? Data(contentsOf: url) else {
            briefEntities = []
            return
        }

        do {
            let languages = try JSONDecoder().decode([BriefEntity].self, from: data)
            briefEntities = filteredBriefLanguages(languages: languages, searchTerm: searchTerm)
        } catch {
            print("JSON parsing error:", error.localizedDescription)
            briefEntities = []
        }
    }
    
    func filteredBriefLanguages(languages: [BriefEntity], searchTerm: String) -> [BriefEntity] {
        let starts = languages.filter { entity in
            entity.name.lowercased().starts(with: searchTerm.lowercased())
        }
        let contains = languages.filter { entity in
            entity.name.lowercased().contains(searchTerm.lowercased())
        }
        return Array(NSOrderedSet(array: starts + contains)) as! [BriefEntity]
    }
    
}
