//
//  SimpleOrder.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation

/// Represents the data structure for sending a new review.
///
/// This struct is used for encoding the body of a POST request to submit a customer review.
struct SendReview: Encodable {
    let itemId: Int
    let customerId: Int
    let text: String
    let rating: Int
}
