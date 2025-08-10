//
//  ItemDetailsView.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 26/06/2025.
//

import SwiftUI

struct ItemDetailsView: View {
    
    var item: Item
    
    @State private var itemViewModel = ItemsViewModel()
    
    @State var isLoaded = false
    
    var body: some View {
        ScrollView {
            getItemImage(item: item)
                .padding(.top, 20)
            Text(item.name)
                .font(.title2)
                .frame(maxWidth: .infinity, alignment: .leading)
                .fontWeight(.medium)
                .padding()
            HStack(spacing: 4) {
                Text(item.price.priceFormat)
                    .italic()
                    .bold()
                Spacer()
                Text("􀋃")
                Text(item.averageRating.ratingFormat)
            }
            .padding(.horizontal)
            .font(.title3)
            if let headline = item.headline {
                getDetail("Headline", headline)
            }
            getDetail("Description", item.description)
            if let authors = item.authors {
                getDetail("Authors", authors.joined(separator: ", "))
            }
            if let topics = item.topics {
                getDetail("Topics", topics.joined(separator: ", "))
            }
            if let genres = item.genres {
                getDetail("Genres", genres.joined(separator: ", "))
            }
            if let numberOfPages = item.numberOfPages {
                getDetail("Number Of Pages", "\(numberOfPages)")
            }
            getDetail("Publisher", item.publisherName)
            getDetail("Publishing Date", Date.basicDateFormatter().string(from: item.publishingDate))
            getDetail("Language", item.language)
            getDetail("Amount In Stock", String(item.amountInStock))
            getDetail("Age Category", "\(item.ageCategory)+")
            if item.type != .typeless {
                getDetail("Item Type", item.type.rawValue)
            }
            if let coverType = item.coverType {
                getDetail("Cover Type", coverType.rawValue)
            }
            getDetail("Condition", "\(item.isUsed ? "Used (\(item.condition?.rawValue ?? "")\((item.hasAnnotations ?? false) ? ", Annotations)" : ")")" : "New \((item.isSealed ?? false) ? "(Sealed)" : "")")").padding(.bottom, 16)
        }
        .frame(width: 240)
    }
    
    func getDetail(_ title: String,_ content: String) -> some View {
        VStack(alignment: .leading) {
            Divider().padding(.bottom, 4)
            VStack(alignment: .leading, spacing: 4) {
                Text("\(title)")
                    .fontWeight(.medium)
                    .font(.footnote)
                    .foregroundStyle(.gray.opacity(0.6))
                Text(content)
            }
        }.padding(.horizontal)
    }
    
    func getItemImage(item: Item) -> some View {
        WebImage(imageURL: item.imageUrl, isLoaded: $isLoaded)
            .overlay(alignment: .bottomTrailing) {
                if (item.isSpecialEdition ?? false) && isLoaded {
                    SpecialEditionBadge()
                        .offset(x: 16, y: 16)
                }
            }
            .frame(width: 200, height: 200)
            .shadow(color: .black.opacity(0.4), radius: 10)
            .padding(.bottom, (item.isSpecialEdition ?? false) ? 20 : 0)
    }
    
}

#Preview {
    ItemDetailsView(item: Item(id: 1, name: "Mutant Message Down Under", description: "A compelling spiritual fiction novel by Marlo Morgan, recounting the journey of an American woman with an Aboriginal tribe in the Australian Outback, sharing profound lessons on nature, spirituality, and humanity.", imageUrl: "https://m.media-amazon.com/images/I/617FyhUdJ7L._AC_UF1000,1000_QL80_.jpg", publishingDate: Date(), language: "English", price: 11.99, amountInStock: 4, publisherId: 0, publisherName: "Penguin Random House", ageCategoryId: 0, ageCategory: 16, averageRating: 4.7, reviews: [], type: .book, isSpecialEdition: true, isUsed: false))
}
