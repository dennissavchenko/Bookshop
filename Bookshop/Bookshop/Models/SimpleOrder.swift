//
//  SimpleOrder.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation
import SwiftUI

/// Represents a simplified overview of an order.
struct SimpleOrder: Identifiable, Decodable {
    let id: Int
    let status: OrderStatus
    let lastUpdatedAt: Date
    let totalPrice: Double
    let customerId: Int
}
