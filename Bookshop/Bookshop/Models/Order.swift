//
//  SimpleOrder.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation
import SwiftUI

/// Represents a customer order.
///
/// This struct is designed to be `Identifiable` for use in SwiftUI lists, and `Decodable`
/// to allow for easy parsing from JSON data received from an API.
struct Order: Identifiable, Decodable {
    let id: Int
    let status: OrderStatus
    let totalPrice: Double
    let confirmedAt: Date
    let preparationStartedAt: Date?
    let shippedAt: Date?
    let deliveredAt: Date?
    let cancelledAt: Date?
    let customerId: Int
    let items: [OrderItem]
}
