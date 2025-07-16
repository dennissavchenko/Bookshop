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
    @State private var text = ""
    @State private var selectedEntity: BriefEntity? = nil
    @FocusState private var isFocused
    @State var model: Model
    @Binding var itemStringValue: String?
    @Binding var itemIntValue: Int?
    
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
                            itemIntValue = entity.id
                            itemStringValue = entity.name
                            isFocused.toggle()
                            text = ""
                            selectedEntity = entity
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
    }
}

#Preview {
    OneSearchView<PublisherViewModel>(title: "Publisher", example: "ex. Penguin Random House", model: PublisherViewModel(), itemStringValue: .constant(nil), itemIntValue: .constant(nil))
}
