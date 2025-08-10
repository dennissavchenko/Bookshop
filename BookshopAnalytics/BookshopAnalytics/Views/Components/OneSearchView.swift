//
//  OneSearchView.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 15/07/2025.
//

import SwiftUI

struct OneSearchView<Model: SearchViewModelProtocol>: View {
    
    let title: String
    let example: String
    let searchEntity: SearchEntity
    @State private var text = ""
    @FocusState private var isFocused
    @State var model: Model
    @State var selectedEntity: BriefEntity? = nil
    @Binding var itemStringValue: String?
    @Binding var itemIntValue: Int?
    @Binding var error: ErrorType
    
    @State var hoveredId = 0
    
    var body: some View {
        Text(title)
            .fontWeight(.medium)
            .font(.footnote)
            .onChange(of: itemIntValue) {
                if itemIntValue != nil {
                    Task {
                        selectedEntity = await model.fetchSelectedEntities(searchEntity: searchEntity, ids: [itemIntValue ?? 0]).first
                    }
                }
            }
            .onChange(of: itemStringValue) {
                if itemStringValue != nil {
                    selectedEntity = BriefEntity(id: 0, name: itemStringValue ?? "")
                }
            }
            .onAppear {
                if itemIntValue != nil {
                    Task {
                        selectedEntity = await model.fetchSelectedEntities(searchEntity: searchEntity, ids: [itemIntValue ?? 0]).first
                    }
                }
                if itemStringValue != nil {
                    selectedEntity = BriefEntity(id: 0, name: itemStringValue ?? "")
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
        if text.trimmingCharacters(in: .whitespaces) != "" && isFocused && !model.briefEntities.isEmpty {
            VStack(alignment: .leading, spacing: 2) {
                ForEach(model.briefEntities, id: \.self) { entity in
                    Text("􀒒  \(entity.name)")
                        .padding(.horizontal, 4)
                        .frame(height: 24)
                        .frame(maxWidth: .infinity, alignment: .leading)
                        .background(hoveredId == entity.id ? Color.gray.opacity(0.2) : Color.clear)
                        .clipShape(RoundedRectangle(cornerRadius: 8))
                        .onTapGesture {
                            itemIntValue = entity.id
                            itemStringValue = entity.name
                            isFocused.toggle()
                            text = ""
                        }
                        .onHover { hovering in
                            hoveredId = hovering ? entity.id : (hoveredId == entity.id ? 0 : hoveredId)
                        }
                    Divider()
                }
            }
        }
        if let selectedEntity = selectedEntity {
            Text("\(selectedEntity.name)")
                .padding(2)
                .italic()
                .padding(.horizontal, 2)
                .foregroundStyle(.white)
                .background {
                    RoundedRectangle(cornerRadius: 0)
                        .fill(Color.accentColor)
                }
        }
        if error != .correct {
            Text("􀁟 \(error.rawValue) \(title.lowercased())!")
                .foregroundStyle(.red)
                .font(.footnote)
        }
    }
}

#Preview {
    OneSearchView<SearchViewModel>(title: "Publisher", example: "ex. Penguin Random House", searchEntity: .publisher, model: SearchViewModel(), itemStringValue: .constant(nil), itemIntValue: .constant(nil), error: .constant(AddItemError().publisherId))
}
