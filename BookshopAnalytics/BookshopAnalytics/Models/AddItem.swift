//
//  SimpleOrder.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation
import SwiftUI

struct AddItem: Identifiable, Decodable, Equatable {
    var id: Int
    var name: String
    var description: String
    var imageUrl: String
    var publishingDate: Date
    var language: String?
    var price: String
    var amountInStock: String
    var publisherId: Int?
    var ageCategoryId: Int?
    var itemType: ItemType?
    var authorsIds: [Int]
    var genresIds: [Int]
    var numberOfPages: String
    var coverType: CoverType?
    var isSpecialEdition: Bool
    var headline: String
    var topics: [String]
    var itemCondition: ItemCondition?
    var isSealed: Bool
    var condition: Condition?
    var hasAnnotations: Bool
}

