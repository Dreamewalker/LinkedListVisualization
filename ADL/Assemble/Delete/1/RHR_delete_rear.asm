aLine 0
gBne Root, Rear, 3

aLine 1
Exception EMPTY_LIST

aLine 3
gNew prevPtr
gMove prevPtr, Root
gNewVPtr nextPtr
gMoveNext nextPtr, Root

aLine 4
gBeq nextPtr, Rear, 5

aLine 5
gMove prevPtr, nextPtr
gMoveNext nextPtr, nextPtr
Jmp -5

aLine 7
gNew delPtr
gMove delPtr, Rear

aLine 8
gMove Rear, prevPtr

aLine 9
nMoveRelOut delPtr, delPtr, 100
pDeleteNext delPtr
pSetNext prevPtr, Root

aLine 10
nDelete delPtr

aLine 11
gDelete delPtr
gDelete prevPtr
gDelete nextPtr
aStd
Halt