//
//  AddNewItem.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 06/07/2025.
//

import SwiftUI

struct AddNewItem: View {
    
    @State var itemsViewModel = ItemsViewModel()
    
    @State var showBadge = false
    @State var isSuccessful = false
    
    @State var item = AddItem(id: 0, name: "", description: "", imageUrl: "", publishingDate: Date(), language: nil, price: "", amountInStock: "", publisherId: nil, ageCategoryId: nil, itemType: ItemType.book, authorsIds: [], genresIds: [], numberOfPages: "", coverType: nil, isSpecialEdition: false, headline: "", topics: [], itemCondition: ItemCondition.new, isSealed: false, condition: nil, hasAnnotations: false)
    
    @State var itemError = AddItemError()
    
    @State var isLoaded = false
    
    @State private var addButtonPressed = false
    
    @Environment(\.dismiss) private var dismiss
    
    var body: some View {
        ScrollView {
            VStack(alignment: .leading, spacing: 8) {
                getTextField("Title", "ex. Left Neglected", $item.name)
                    .onChange(of: item.name) {
                        item.name = String(item.name.prefix(100))
                    }
                getTextField("Description", "ex. Left Neglected tells the story of Sarah Nickerson, a driven career woman and mother...", $item.description)
                    .onChange(of: item.description) {
                        item.description = String(item.description.prefix(300))
                    }
                getTextField("Image URL", "ex. https://amazon.com/images/left_neglected.jpg", $item.imageUrl, isImageURL: true)
                    .onChange(of: item.imageUrl) {
                        item.imageUrl = String(item.imageUrl.prefix(300))
                    }
                if !item.imageUrl.isEmptyW {
                    WebImage(imageURL: item.imageUrl, isLoaded: $isLoaded)
                        .scaledToFill()
                        .frame(width: 200)
                }
                Text("Publishing Date")
                    .fontWeight(.medium)
                    .font(.footnote)
                DatePicker("", selection: $item.publishingDate, displayedComponents: .date)
                    .datePickerStyle(.automatic)
                    .labelsHidden()
                if itemError.publishingDate != .correct {
                    Text("􀁟 \(itemError.publishingDate.rawValue)")
                        .foregroundStyle(.red)
                        .font(.footnote)
                }
                OneSearchView<SearchViewModel>(title: "Publisher", example: "ex. Penguin Random House", searchEntity: .publisher, model: SearchViewModel(), itemStringValue: .constant(nil), itemIntValue: $item.publisherId, error: $itemError.publisherId)
                OneSearchView<LanguageViewModel>(title: "Language", example: "ex. English", searchEntity: .language, model: LanguageViewModel(), itemStringValue: $item.language, itemIntValue: .constant(nil), error: $itemError.language)
                DropDownListView(title: "Age Category", selectedId: $item.ageCategoryId, error: $itemError.ageCategoryId)
                getTextField("Price", "ex. 11.99", $item.price)
                    .onChange(of: item.price) { oldValue, newValue in
                        if !newValue.validatePriceString() && !newValue.isEmpty {
                            item.price = oldValue
                        }
                    }
                getTextField("Amount In Stock", "ex. 12", $item.amountInStock)
                    .onChange(of: item.amountInStock) { oldValue, newValue in
                        if !newValue.validateAmountString() && !newValue.isEmpty {
                            item.amountInStock = oldValue
                        }
                    }
                HorizontalRadioButtons(title: "Item Type", selectedOption: $item.itemType)
                if item.itemType == .book {
                    MultiSearchView<SearchViewModel>(title: "Authors", example: "ex. Lisa Genova", searchEntity: .author, model: SearchViewModel(), itemIdArray: $item.authorsIds, error: $itemError.authorsIds)
                    MultiSearchView<SearchViewModel>(title: "Genres", example: "ex. Medical Drama", searchEntity: .genre, model: SearchViewModel(), itemIdArray: $item.genresIds, error: $itemError.genresIds)
                    // mask
                    getTextField("Number Of Pages", "ex. 431", $item.numberOfPages, .book)
                    getErrorText(itemError.numberOfPages)
                    VerticalRadioButtons<CoverType>(title: "Cover Type", selectedOption: $item.coverType, error: $itemError.coverType)
                } else if item.itemType == .magazine {
                    getToggle("Question", "Is this item a special edition?", $item.isSpecialEdition)
                } else if item.itemType == .newspaper {
                    getTextField("Headline", "ex. Men Walk On Moon", $item.headline, .newspaper)
                    getErrorText(itemError.headline)
                    MultiStringView(title: "Topics", example: "ex. Finances", stringList: $item.topics, error: $itemError.topics)
                }
                HorizontalRadioButtons<ItemCondition>(title: "Item Condition", selectedOption: $item.itemCondition)
                if item.itemCondition == .new {
                    getToggle("Question", "Is this item sealed?", $item.isSealed)
                } else {
                    VerticalRadioButtons<Condition>(title: "Wear Level", selectedOption: $item.condition, error: $itemError.condition)
                    getToggle("Question", "Does this item have annotations?", $item.hasAnnotations)
                }
                HStack {
                    Button(item.id == 0 ? "Add" : "Update") {
                        var result = false
                        addButtonPressed = true
                        if !item.price.validatePriceString() {
                            item.price = ""
                        }
                        if !item.amountInStock.validateAmountString() {
                            item.amountInStock = ""
                        }
                        if !item.numberOfPages.validateAmountString() {
                            item.numberOfPages = ""
                        }
                        (result, itemError) = (validateNewItem(item))
                        if result {
                            print(item)
                            Task {
                                if item.id == 0 {
                                    if await itemsViewModel.addItem(item: item) == 201 {
                                        isSuccessful = true
                                        withAnimation {
                                            showBadge = true
                                        }
                                        DispatchQueue.main.asyncAfter(deadline: .now() + 3) {
                                            dismiss()
                                        }
                                    } else {
                                        isSuccessful = false
                                        withAnimation {
                                            showBadge = true
                                        }
                                        DispatchQueue.main.asyncAfter(deadline: .now() + 3) {
                                            withAnimation {
                                                showBadge = false
                                            }
                                        }
                                    }
                                } else {
                                    if await itemsViewModel.updateItem(item: item) == 204 {
                                        isSuccessful = true
                                        withAnimation {
                                            showBadge = true
                                        }
                                        DispatchQueue.main.asyncAfter(deadline: .now() + 3) {
                                            dismiss()
                                        }
                                    } else {
                                        isSuccessful = false
                                        withAnimation {
                                            showBadge = true
                                        }
                                        DispatchQueue.main.asyncAfter(deadline: .now() + 3) {
                                            withAnimation {
                                                showBadge = false
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }.buttonStyle(.borderedProminent)
                    Button("Cancel") {
                        dismiss()
                    }
                }.padding(.top)
            }
            .padding()
        }
        .overlay {
            if showBadge {
                CompletionBadge(isSuccessful: isSuccessful)
            }
        }
        .onChange(of: item) { new, old in
            if addButtonPressed && new.itemType == old.itemType && new.itemCondition == old.itemCondition {
                (_, itemError) = validateNewItem(item)
            }
        }
    }
    
    func getTextField(_ title: String, _ example: String, _ text: Binding<String>, _ itemType: ItemType = .typeless, isImageURL: Bool = false) -> some View {
        VStack(alignment: .leading) {
            Text(title)
                .fontWeight(.medium)
                .font(.footnote)
            TextField(example, text: text)
            if addButtonPressed && text.wrappedValue.isEmptyW && itemType == .typeless {
                Text("􀁟 This field cannot be empty!")
                    .foregroundStyle(.red)
                    .font(.footnote)
            } else if addButtonPressed && !isLoaded && itemType == .typeless && isImageURL {
                Text("􀁟 Invalid image URL")
                    .foregroundStyle(.red)
                    .font(.footnote)
            }
        }
    }
    
    func getToggle(_ title: String, _ question: String, _ boolean: Binding<Bool>) -> some View {
        VStack(alignment: .leading) {
            Text(title)
                .fontWeight(.medium)
                .font(.footnote)
            Toggle(question, isOn: boolean)
        }
    }
    
    @ViewBuilder
    func getErrorText(_ error: ErrorType) -> some View {
        if error != .correct {
            Text("􀁟 \(error.rawValue)")
                .foregroundStyle(.red)
                .font(.footnote)
        }
    }
    
    func validateNewItem(_ item: AddItem) -> (Bool, AddItemError) {
        var result = true
        var itemError = AddItemError()
        if item.name.isEmptyW || item.description.isEmptyW ||
            item.imageUrl.isEmpty || !isLoaded || item.price.isEmptyW || item.amountInStock.isEmptyW {
            result = false
        }
        if item.publishingDate > Date.now {
            itemError.publishingDate = .futureDate
            result = false
        }
        if item.publisherId == nil {
            itemError.publisherId = .zeroChoiceOne
            result = false
        }
        if item.ageCategoryId == nil {
            itemError.ageCategoryId = .zeroChoiceOne
            result = false
        }
        if item.language == nil {
            itemError.language = .zeroChoiceOne
            result = false
        }
        if item.authorsIds.isEmpty && item.itemType == .book {
            itemError.authorsIds = .zeroChoice
            result = false
        }
        if item.genresIds.isEmpty && item.itemType == .book {
            itemError.genresIds = .zeroChoice
            result = false
        }
        if item.coverType == nil && item.itemType == .book {
            itemError.coverType = .zeroChoiceOne
            result = false
        }
        if item.numberOfPages.isEmptyW && item.itemType == .book {
            itemError.numberOfPages = .empty
            result = false
        }
        if item.headline.isEmptyW && item.itemType == .newspaper {
            itemError.headline = .empty
            result = false
        }
        if item.topics.isEmpty && item.itemType == .newspaper {
            itemError.topics = .zeroChoice
            result = false
        }
        if item.condition == nil && item.itemCondition == .used {
            itemError.condition = .zeroChoiceOne
            result = false
        }
        return (result, itemError)
    }
    
}

#Preview {
    AddNewItem()
}
