//
//  ItemView.swift
//  BookShop
//
//  Created by dennis savchenko on 10/06/2025.
//

import SwiftUI

struct ItemView: View {
    
    @State private var model = ItemViewModel()
    
    let itemId: Int
    let customerId: Int
    
    @State var isLoaded: Bool = false
    @State var addingReview = false
    
    var customerLeftReview: Bool? {
        model.item?.reviews.contains(where: { review in
            review.customerId == customerId
        })
    }
    
    var body: some View {
        ScrollView {
            VStack(alignment: .leading, spacing: 20) {
                if model.isLoading {
                    ProgressView("Loading...")
                } else if let item = model.item {
                    VStack(spacing: 20) {
                        itemImage(item: item, isLoaded: isLoaded)
                        header(item: item)
                        HStack(spacing: 12) {
                            Button {
                               //
                            } label: {
                                HStack(spacing: 2) {
                                    Image(systemName: "cart.fill")
                                        .fontWeight(.medium)
                                    Text("Add To Cart")
                                        .fontWeight(.bold)
                                }
                                .foregroundStyle(.white)
                                .padding()
                                .frame(maxWidth: .infinity)
                                .background {
                                    RoundedRectangle(cornerRadius: 12)
                                }
                            }
                            Text("only \(item.amountInStock) left")
                                .foregroundStyle(.gray)
                                .font(.system(size: 14))
                                .underline()
                        }.frame(maxWidth: .infinity)
                        partTitle(title: "DESCRIPTION")
                            .padding(.top, 32)
                        HStack {
                            Text("\(item.description)")
                        }
                        .frame(maxWidth: .infinity, alignment: .leading)

                        partTitle(title: "DETAILS")
                        details(item: item)
                        if item.reviews.count > 0 {
                            partTitle(title: "REVIEWS")
                        }
                        ForEach(item.reviews) { review in
                            ReviewListView(review: review)
                            Divider()
                        }
                        if (model.customerReceivedItem ?? false) && !(customerLeftReview ?? false) {
                            NavigationLink {
                                NewReviewView(customerId: customerId, itemId: itemId)
                            } label: {
                                HStack(spacing: 2) {
                                    Image(systemName: "star.fill")
                                        .fontWeight(.medium)
                                    Text("Leave Review")
                                        .fontWeight(.bold)
                                }
                                .foregroundStyle(.black)
                                .padding()
                                .frame(maxWidth: .infinity)
                                .background {
                                    RoundedRectangle(cornerRadius: 12)
                                        .stroke(.black, lineWidth: 3)
                                }
                            }
                        }
                    }
                }
            }
            .padding()
        }
        .navigationTitle("Item Details")
        .navigationBarTitleDisplayMode(.inline)
        .kerning(-0.3)
        .scrollIndicators(.never)
        .refreshable {
            Task {
                await model.loadItem(itemId: itemId)
                await model.hasReceivedItem(customerId: customerId, itemId: itemId)
            }
        }
        .task {
            await model.loadItem(itemId: itemId)
            await model.hasReceivedItem(customerId: customerId, itemId: itemId)
        }
    }
    
    func header(item: Item) -> some View {
        VStack {
            VStack(alignment: .leading, spacing: 4) {
                Text(item.name)
                    .font(.largeTitle)
                    .fontWeight(.semibold)
                Text(item.authors?.joined(separator: ", ") ?? item.publisherName)
                    .foregroundStyle(.gray)
                    .font(.title3)
            HStack(alignment: .bottom) {
                HStack(spacing: 4) {
                    Image(systemName: "star.fill")
                    Text("\(String(format: "%.1f", item.averageRating)) ")
                        .fontWeight(.semibold)
                        .font(.title3) + Text("(\(item.reviews.count) \(item.reviews.count == 1 ? "review)" : "reviews)")")
                        .underline()
                }
                .padding(.top, 8)
                .foregroundStyle(.star)
                Spacer()
                VStack(alignment: .trailing) {
                    Text("Price:")
                        .fontWeight(.semibold)
                    Text(Double.getPriceString(price: item.price, currency: "$"))
                        .fontWeight(.semibold)
                        .italic()
                        .font(.title)
                        .padding(.bottom, -4)
                    }
                }
            }
        }
    }
    
    func details(item: Item) -> some View {
        VStack(alignment: .leading, spacing: 8) {
            if let genres = item.genres {
                detailItem(title: "Genres", content: genres.joined(separator: ", "))
            }
            if let headline = item.headline {
                detailItem(title: "Headline", content: headline)
            }
            
            detailItem(title: "Publisher", content: item.publisherName)
            if let numberOfPages = item.numberOfPages {
                detailItem(title: "Number of Pages", content: "\(numberOfPages)")
            }
            if let coverType = item.coverType {
                detailItem(title: "Cover Type", content: coverType.rawValue)
            }
            detailItem(title: "Publishing Date", content: Date.fullDateFormatter().string(from: item.publishingDate))
            detailItem(title: "Language", content: item.language)
            detailItem(title: "Age Category", content: "\(item.ageCategory)+")
            detailItem(title: "Condition", content: "\(item.isUsed ? "Used (\(item.condition?.rawValue ?? "")\((item.hasAnnotations ?? false) ? ", Annotations)" : ")")" : "New \((item.isSealed ?? false) ? "(Sealed)" : "")")")
            if let topics = item.topics {
                detailItem(title: "Topics", content: topics.joined(separator: ", "))
            }
        }
    }
    
    func itemImage(item: Item, isLoaded: Bool) -> some View {
        WebImage(imageURL: item.imageUrl, isLoaded: $isLoaded)
            .scaledToFit()
            .frame(height: 332)
            .clipShape(RoundedRectangle(cornerRadius: 8))
            .shadow(color: .black.opacity(0.4), radius: 10)
            .overlay(alignment: .bottomTrailing) {
                if (item.isSpecialEdition ?? false) && isLoaded {
                    specialEditionBadge
                        .offset(x: 30, y: 30)
                }
            }
            .padding(.vertical, 24)
            .padding(.bottom, (item.isSpecialEdition ?? false) ? 20 : 0)
    }
    
    func detailItem(title: String, content: String) -> some View {
        VStack {
            HStack {
                Text("\(title): ")
                    .fontWeight(.semibold)
                + Text(content)
                Spacer()
            }
            Divider()
        }
    }
    
    func partTitle(title: String) -> some View {
        HStack {
            Text(title)
                .foregroundStyle(.gray)
            Spacer()
        }
    }
    
    var specialEditionBadge: some View {
        HStack {
            Text("Special Edition")
            Image(systemName: "star.circle.fill")
        }
        .fontWeight(.bold)
        .foregroundStyle(.white)
        .padding()
        .background {
            Image("SpecialEdition")
                .resizable()
        }
        .clipShape(RoundedRectangle(cornerRadius: 12))
    }
    
}

#Preview {
    ItemView(itemId: 2, customerId: 1)
}
