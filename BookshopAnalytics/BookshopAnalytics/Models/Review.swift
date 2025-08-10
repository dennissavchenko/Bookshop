import Foundation
import SwiftUI

struct Review: Identifiable, Decodable {
    let id: Int
    let text: String
    let rating: Int
    let timeStamp: Date
    let username: String
    let itemId: Int
    let customerId: Int
}
