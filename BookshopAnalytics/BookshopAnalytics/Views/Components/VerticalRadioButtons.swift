//
//  HorizontalRadioButtons.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 06/07/2025.
//

import SwiftUI

struct VerticalRadioButtons<T>: View where T: CaseIterable & Hashable & RawRepresentable, T.RawValue == String  {
    
    let title: String
    @Binding var selectedOption: T?
    @Binding var error: ErrorType

    var body: some View {
        VStack(alignment: .leading) {
            Text(title)
                .fontWeight(.medium)
                .font(.footnote)

            VStack(alignment: .leading, spacing: 8) {
                ForEach(Array(T.allCases), id: \.self) { option in
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
                        Text(option.rawValue == "SpiralBound" ? "Spiral Bound" : option.rawValue)
                    }
                    .background(.clear)
                    .onTapGesture {
                        withAnimation {
                            selectedOption = option
                        }
                    }
                }
            }
            if error != .correct {
                Text("􀁟 \(error.rawValue) \(title.lowercased())!")
                    .foregroundStyle(.red)
                    .font(.footnote)
                    .padding(.top, 2)
            }
        }
    }
}

#Preview {
    VerticalRadioButtons<CoverType>(title: "Cover Type", selectedOption: .constant(CoverType.hard), error: .constant(AddItemError().coverType))
}
