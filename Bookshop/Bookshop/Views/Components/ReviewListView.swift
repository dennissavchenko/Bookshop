//
//  ReviewListView.swift
//  BookShop
//
//  Created by dennis savchenko on 10/06/2025.
//

import SwiftUI

struct ReviewListView: View {
    
    let review: Review
    
    var body: some View {
        VStack(alignment: .leading, spacing: 16) {
            HStack(spacing: 4) {
                Text(review.username)
                    .fontWeight(.semibold)
                Spacer()
                HStack(spacing: 0) {
                    Image(systemName: review.rating >= 1 ? "star.fill" : "star")
                    Image(systemName: review.rating >= 2 ? "star.fill" : "star")
                    Image(systemName: review.rating >= 3 ? "star.fill" : "star")
                    Image(systemName: review.rating >= 4 ? "star.fill" : "star")
                    Image(systemName: review.rating >= 5 ? "star.fill" : "star")
                }.foregroundStyle(.star)
                Text("\(review.rating)/5")
                    .fontWeight(.semibold)
                    .foregroundStyle(.star)
            }
            Text(review.text)
                .font(.system(size: 15))
            HStack {
                Spacer()
                Text("left on \(Date.dateBasicFormatter().string(from: review.timeStamp))")
                    .foregroundStyle(.gray)
                    .font(.footnote)
            }
        }
    }
}

#Preview {
    ReviewListView(review: Review(id: 1, text: "A gripping and atmospheric read. Wool pulls you into a claustrophobic world full of secrets and suspense. The pacing drags a bit in places, but Juliette is a strong, compelling lead and the mystery kept me hooked.", rating: 4, timeStamp: Date.now, username: "dennissavchenko", itemId: 1, customerId: 1))
}
