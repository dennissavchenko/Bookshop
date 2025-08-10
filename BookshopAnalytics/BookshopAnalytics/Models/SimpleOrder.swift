import Foundation
import SwiftUI

struct SimpleOrder: Identifiable, Decodable, Hashable {
    let id: Int
    let status: OrderStatus
    let lastUpdatedAt: Date
    let totalPrice: Double
    let customerId: Int
}
