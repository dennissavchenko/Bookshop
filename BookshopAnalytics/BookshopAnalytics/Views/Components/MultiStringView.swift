//
//  OneSearchView.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 15/07/2025.
//

import SwiftUI

struct MultiStringView: View {
    
    let title: String
    let example: String
    @Binding var stringList: [String]
    @State var text: String = ""
    @FocusState var isFocused
    @Binding var error: ErrorType
    
    var body: some View {
        Text(title)
            .fontWeight(.medium)
            .font(.footnote)
        TextField(example, text: $text)
            .focused($isFocused)
            .onSubmit {
                if !stringList.contains(where: { $0 == text }) {
                    stringList.append(text)
                    text = ""
                }
            }
        if !stringList.isEmpty {
            MultiStringGridView(stringArray: $stringList)
        }
        if error != .correct {
            Text("􀁟 \(error.rawValue) \(title.lowercased().dropLast())!")
                .foregroundStyle(.red)
                .font(.footnote)
        }
    }
}

#Preview {
    MultiStringView(title: "Topics", example: "ex. AI", stringList: .constant([]), error: .constant(AddItemError().topics))
}
