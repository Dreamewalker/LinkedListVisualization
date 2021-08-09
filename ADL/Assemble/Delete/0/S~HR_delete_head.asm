aLine 0
gNew delPtr
gMove delPtr, Root

aLine 1
gBne Root, null, 3

aLine 2
Exception EMPTY_LIST

aLine 4
gMoveNext Root, Root

aLine 5
pDeleteNext delPtr
nDelete delPtr
gDelete delPtr

aLine 6
aStd
Halt