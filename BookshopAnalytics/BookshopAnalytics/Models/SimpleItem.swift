//
//  SimpleItem.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation
import SwiftUI

/// Represents a simplified version of an item, suitable for display in lists or overviews.
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
