import Foundation
import SwiftUI

struct SimpleItem: Identifiable, Decodable, Hashable {
    let id: Int
    let name: String
    let imageUrl: String
    let price: Double
    let publisherName: String
    let averageRating: Double
    let authors: [String]?
    let genres: [String]?
}
