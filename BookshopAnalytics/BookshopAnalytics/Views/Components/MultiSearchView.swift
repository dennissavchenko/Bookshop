//
//  OneSearchView.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 15/07/2025.
//

import SwiftUI

struct MultiSearchView<Model: SearchViewModelProtocol>: View {
    
    let title: String
    let example: String
    @State private var text = ""
    @State private var selectedEntities: [BriefEntity] = []
    @FocusState private var isFocused
    @State var model: Model
    @Binding var itemIdArray: [Int]
    
    var body: some View {
        Text(title)
            .fontWeight(.medium)
            .font(.footnote)
        TextField(example, text: $text)
            .focused($isFocused)
            .onChange(of: text) {
                if isFocused && text.trimmingCharacters(in: .whitespaces) != "" {
                    Task {
                        await model.fetchBriefEntities(searchTerm: text)
                    }
                }
            }
        if isFocused && text != "" {
            VStack(alignment: .leading) {
                ForEach(model.briefEntities, id: \.self) { entity in
                    Text("􀒒  \(entity.name)")
                        .frame(maxWidth: .infinity, alignment: .leading)
                        .contentShape(Rectangle())
                        .onTapGesture {
                            if !itemIdArray.contains(entity.id) {
                                itemIdArray.append(entity.id)
                                text = ""
                                selectedEntities.append(entity)
                            } else {
                                isFocused.toggle()
                                text = ""
                            }
                        }
                    Divider()
                }
            }
        }
        MultiChoiceGridView(briefEntities: $selectedEntities, idArray: $itemIdArray)
    }
}

#Preview {
    MultiSearchView<AuthorViewModel>(title: "Authors", example: "ex. Lisa Genova", model: AuthorViewModel(), itemIdArray: .constant([]))
}
