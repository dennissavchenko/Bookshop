//
//  Order.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 10/08/2025.
//

import Foundation
import SwiftUI

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

struct OrderItem: Decodable {
    let item: SimpleItem
    let quantity: Int
}
