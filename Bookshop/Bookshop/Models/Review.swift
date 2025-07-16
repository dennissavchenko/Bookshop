//
//  SimpleOrder.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation
import SwiftUI

/// Represents a customer review for an item.
struct Review: Identifiable, Decodable {
    let id: Int
    let text: String
    let rating: Int
    let timeStamp: Date
    let username: String
    let itemId: Int
    let customerId: Int
}
