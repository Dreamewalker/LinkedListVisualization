aLine 0
gNew delPtr
gMove delPtr, Root

aLine 1
gBne Root, null, 3

aLine 2
Exception EMPTY_LIST

aLine 4
gBne Root, Rear, 6

aLine 5
gMove Root, null

aLine 6
gMove Rear, null
Jmp 8

aLine 9
nMoveRelOut Root, Root, 100
gMoveNext Root, Root

aLine 10
pSetNext Rear, Root

aLine 12
pDeleteNext delPtr
nDelete delPtr
gDelete delPtr

aLine 13
aStd
Halt