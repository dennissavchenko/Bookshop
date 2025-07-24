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
    let searchEntity: SearchEntity
    @State private var text = ""
    @State private var selectedEntities: [BriefEntity] = []
    @FocusState private var isFocused
    @State var model: Model
    @Binding var itemIdArray: [Int]
    
    var body: some View {
        Text(title)
            .fontWeight(.medium)
            .font(.footnote)
            .onChange(of: itemIdArray) {
                if !itemIdArray.isEmpty {
                    Task {
                        selectedEntities = await model.fetchSelectedEntities(searchEntity: searchEntity, ids: itemIdArray)
                    }
                }
            }
            .onAppear {
                if !itemIdArray.isEmpty {
                    Task {
                        selectedEntities = await model.fetchSelectedEntities(searchEntity: searchEntity, ids: itemIdArray)
                    }
                }
            }
        TextField(example, text: $text)
            .focused($isFocused)
            .onChange(of: text) {
                if isFocused && text.trimmingCharacters(in: .whitespaces) != "" {
                    Task {
                        await model.fetchBriefEntities(searchEntity: searchEntity, searchTerm: text)
                    }
                }
            }
        if isFocused && !model.briefEntities.isEmpty && text.trimmingCharacters(in: .whitespaces) != "" {
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
        if !selectedEntities.isEmpty {
            MultiChoiceGridView(briefEntities: $selectedEntities, idArray: $itemIdArray)
        }
    }
}

#Preview {
    MultiSearchView<SearchViewModel>(title: "Authors", example: "ex. Lisa Genova", searchEntity: .author, model: SearchViewModel(), itemIdArray: .constant([]))
}
