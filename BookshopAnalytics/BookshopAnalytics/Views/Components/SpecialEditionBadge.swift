//
//  SpecialEditionBadge.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 06/07/2025.
//

import SwiftUI

struct SpecialEditionBadge: View {
    var body: some View {
        HStack {
            Text("Special Edition")
            Image(systemName: "star.circle.fill")
        }
        .fontWeight(.medium)
        .foregroundStyle(.white)
        .padding(8)
        .background {
            Image("SpecialEdition")
                .resizable()
        }
        .clipShape(RoundedRectangle(cornerRadius: 8))
    }
}

#Preview {
    SpecialEditionBadge()
}
