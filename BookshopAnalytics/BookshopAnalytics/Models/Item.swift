import Foundation
import SwiftUI

struct Item: Identifiable, Decodable {
    var id: Int
    var name: String
    var description: String
    var imageUrl: String
    var publishingDate: Date
    var language: String
    var price: Double
    var amountInStock: Int
    var publisherId: Int
    var publisherName: String
    var ageCategoryId: Int
    var ageCategory: Int
    var averageRating: Double
    var reviews: [Review]
    var type: ItemType
    var authorsIds: [Int]?
    var genresIds: [Int]?
    var authors: [String]?
    var genres: [String]?
    var numberOfPages: Int?
    var coverType: CoverType?
    var isSpecialEdition: Bool?
    var headline: String?
    var topics: [String]?
    var isUsed: Bool
    var isSealed: Bool?
    var condition: Condition?
    var hasAnnotations: Bool?
}

