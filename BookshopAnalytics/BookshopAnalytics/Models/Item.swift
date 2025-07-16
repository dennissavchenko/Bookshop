//
//  SimpleOrder.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation
import SwiftUI

/// Represents a generic item available in the bookshop, including books, magazines, and newspapers.
///
/// This struct is designed to be `Identifiable` for use in SwiftUI lists, and `Decodable`
/// to allow for easy parsing from JSON data received from an API.
struct Item: Identifiable, Decodable {
    var id: Int
    var name: String
    var description: String
    var imageUrl: String
    var publishingDate: Date
    var language: String
    var price: Double
    var amountInStock: Int
    var publisherName: String
    var ageCategory: Int
    var averageRating: Double
    var reviews: [Review]
    var type: ItemType
    var authors: [String]?
    var genres: [String]?
    var numberOfPages: Int?
    var coverType: CoverType?
    var isSpecialEdition: Bool?
    var headline: String?
    var topics: [String]?
    var isUsed: Bool
    var isSealed: Bool?
    var condition: Condition?
    var hasAnnotations: Bool?
}

