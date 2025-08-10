//
//  SimpleOrder.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation
import SwiftUI

enum ErrorType: String, CaseIterable {
    case empty = "This field cannot be empty!"
    case zeroChoice = "You have to select at least one"
    case zeroChoiceOne = "You have to select one"
    case badUrl = "This URL is invalid!"
    case futureDate = "Date cannot be in the future!"
    case invalidFormat = "Invalid format!"
    case correct = ""
}

struct AddItemError {
    var name: ErrorType = .correct
    var description: ErrorType = .correct
    var imageUrl: ErrorType = .correct
    var publishingDate: ErrorType = .correct
    var language: ErrorType = .correct
    var price: ErrorType = .correct
    var amountInStock: ErrorType = .correct
    var publisherId: ErrorType = .correct
    var ageCategoryId: ErrorType = .correct
    var itemType: ErrorType = .correct
    var authorsIds: ErrorType = .correct
    var genresIds: ErrorType = .correct
    var numberOfPages: ErrorType = .correct
    var coverType: ErrorType = .correct
    var isSpecialEdition: ErrorType = .correct
    var headline: ErrorType = .correct
    var topics: ErrorType = .correct
    var itemCondition: ErrorType = .correct
    var isSealed: ErrorType = .correct
    var condition: ErrorType = .correct
    var hasAnnotations: ErrorType = .correct
}

