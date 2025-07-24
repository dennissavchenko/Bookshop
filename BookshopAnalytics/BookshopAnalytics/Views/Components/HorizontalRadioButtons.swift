//
//  HorizontalRadioButtons.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 06/07/2025.
//

import SwiftUI

struct HorizontalRadioButtons: View {
    
    @Binding var selectedOption: ItemType?
    
    let options = [ItemType.book, ItemType.magazine, ItemType.newspaper, ItemType.typeless]

    var body: some View {
        VStack(alignment: .leading) {
            Text("Item Type")
                .fontWeight(.medium)
                .font(.footnote)

            HStack(spacing: 20) {
                ForEach(options, id: \.self) { option in
                    HStack {
                        ZStack {
                            Circle()
                                .stroke(Color.gray, lineWidth: selectedOption == option ? 0 : 0.5)
                                .fill(selectedOption == option ? Color.accentColor : Color.white)
                                .frame(width: 16, height: 16)
                            Circle()
                                .fill(.white)
                                .frame(width: 6, height: 6)
                        }
                        Text(option == .typeless ? "Other" : option.rawValue)
                    }
                    .background(.clear)
                    .onTapGesture {
                        selectedOption = option
                    }
                }
            }
        }
    }
}

#Preview {
    HorizontalRadioButtons(selectedOption: .constant(ItemType.book))
}
