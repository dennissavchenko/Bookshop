//
//  UserTokens.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 24/06/2025.
//

struct UserTokens : Decodable, Encodable {
    var id: Int
    var accessToken: String
    var refreshToken: String
}
