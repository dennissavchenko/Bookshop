//
//  ISearchViewModel.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 15/07/2025.
//

import Foundation

protocol SearchViewModelProtocol {
    var briefEntities: [BriefEntity] { get set }
    func fetchBriefEntities(searchTerm: String) async
}
