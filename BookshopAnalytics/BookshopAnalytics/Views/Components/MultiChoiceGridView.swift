//
//  MultiChoiceGrid.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 16/07/2025.
//

import SwiftUI

struct MultiChoiceGridView: View {
    
    @Binding var briefEntities: [BriefEntity]
    @Binding var idArray: [Int]

    var body: some View {
        AnyLayout(FlowLayout(spacing: 8)) {
            ForEach(briefEntities) { briefEntity in
                HStack {
                    Image(systemName: "xmark")
                        .onTapGesture {
                            briefEntities.removeAll(where: { $0.id == briefEntity.id })
                            idArray.removeAll(where: { $0 == briefEntity.id } )
                        }
                    Text("\(briefEntity.name)")
                }
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
}

struct FlowLayout: Layout {
    
    var spacing: CGFloat
    
    func layout(sizes: [CGSize], spacing: CGFloat, containerWidth: CGFloat) -> (offsets: [CGPoint], size: CGSize) {
        
        var result: [CGPoint] = []

        var currentPosition: CGPoint = .zero

        var lineHeight: CGFloat = 0

        var maxX: CGFloat = 0
        
        for size in sizes {
            if currentPosition.x + size.width > containerWidth {
                 currentPosition.x = 0
                 currentPosition.y += lineHeight + spacing
                 lineHeight = 0
             }
             
             result.append(currentPosition)
             currentPosition.x += size.width

             maxX = max(maxX, currentPosition.x)
             currentPosition.x += spacing
             lineHeight = max(lineHeight, size.height)
        }
        
        return (result, .init(width: maxX, height: currentPosition.y + lineHeight))
        
     }
 
     func sizeThatFits(proposal: ProposedViewSize,
                       subviews: Subviews,
                       cache: inout ()) -> CGSize {

        let containerWidth = proposal.width ?? .infinity
         let sizes = subviews.map { $0.sizeThatFits(.unspecified) }
         return layout(sizes: sizes,
                       spacing: spacing,
                       containerWidth: containerWidth).size
     }
     
     func placeSubviews(in bounds: CGRect,
                        proposal: ProposedViewSize,
                        subviews: Subviews,
                        cache: inout ()) {
         let sizes = subviews.map { $0.sizeThatFits(.unspecified) }
         let offsets =
             layout(sizes: sizes,
                    spacing: spacing,
                    containerWidth: bounds.width).offsets
         for (offset, subview) in zip(offsets, subviews) {
             subview.place(at: .init(x: offset.x + bounds.minX,
                                     y: offset.y + bounds.minY),
                           proposal: .unspecified)
         }
     }
 }

#Preview {
    MultiChoiceGridView(briefEntities: .constant([BriefEntity(id: 1, name: "Lisa Genova"), BriefEntity(id: 2, name: "Marlo Morgan"), BriefEntity(id: 3, name: "Marlo Morgan")]), idArray: .constant([]))
}
