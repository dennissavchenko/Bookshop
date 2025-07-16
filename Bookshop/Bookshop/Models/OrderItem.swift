//
//  SimpleOrder.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation
import SwiftUI

/// Represents a single item within an order, including its quantity.
struct OrderItem: Decodable {
    let item: SimpleItem
    let quantity: Int
}
