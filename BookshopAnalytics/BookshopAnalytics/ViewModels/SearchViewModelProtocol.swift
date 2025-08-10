import Foundation

protocol SearchViewModelProtocol {
    var briefEntities: [BriefEntity] { get set }
    func fetchBriefEntities(searchEntity: SearchEntity, searchTerm: String) async
    func fetchSelectedEntities(searchEntity: SearchEntity, ids: [Int]) async -> [BriefEntity]
}
