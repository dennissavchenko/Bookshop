import Foundation

@MainActor
@Observable
class LanguageViewModel: @preconcurrency SearchViewModelProtocol {
    
    var briefEntities: [BriefEntity] = []
    
    var statusCode: Int?
    
    func fetchBriefEntities(searchEntity: SearchEntity, searchTerm: String) async {
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
    
    func fetchSelectedEntities(searchEntity: SearchEntity, ids: [Int]) async -> [BriefEntity] {
        return []
    }
    
}
