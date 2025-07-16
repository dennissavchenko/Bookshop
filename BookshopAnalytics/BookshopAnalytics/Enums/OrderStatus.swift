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
    
    var intValue: Int {
        switch self {
        case .cart: return 0
        case .pending: return 1
        case .confirmed: return 2
        case .preparation: return 3
        case .shipped: return 4
        case .delivered: return 5
        case .cancelled: return 6
        }
    }
    
}
