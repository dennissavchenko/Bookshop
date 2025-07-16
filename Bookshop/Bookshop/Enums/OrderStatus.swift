//
//  OrderStatus.swift
//  BookShop
//
//  Created by dennis savchenko on 10/06/2025.
//

/// Represents the possible statuses of an order.
///
/// This enum conforms to `String`, `Decodable`, and `CaseIterable`
/// to facilitate easy serialization/deserialization and iteration.
enum OrderStatus: String, Decodable, CaseIterable {
    case cart = "Cart"
    case pending = "Pending"
    case confirmed = "Confirmed"
    case preparation = "Preparation"
    case shipped = "Shipped"
    case delivered = "Delivered"
    case cancelled = "Cancelled"
}
