# LinkedListVisualization
2020-2021学年春季学期暑假 数据结构课设1——链表可视化（基于WPF）

## 动画描述指令(ADL)
### 通用指针操作
gNew newPtr, (CanvasLeft, CanvasTop)   // 新建一个指针newPtr, 后两个参数指示了新指针的位置，可选  
gMove dstPtr, srcPtr   // 将指针dstPtr指向srcPtr指向的结点  
gDelete dstPtr    // 删除dstPtr  
gNewVPtr newPtr     // 新建一个不可见的指针newPtr  
gMoveNext dstPtr, srcPtr    // 将dstPtr指向srcPtr指向的结点的后继   
gMovePrev dstPtr, srcPtr    // 将dstPtr指向srcPtr指向的结点的前驱  

### 前驱/后继指针操作
pSetNext dstPtr, srcPtr  // 将指针dstPtr指向的结点的后继指针指向srcPtr指向的结点  
pSetPrev dstPtr, srcPtr  // 将指针dstPtr指向的结点的前驱指针指向srcPtr指向的结点  
pDeleteNext dstPtr  // 删除指针dstPtr指向的结点的后继指针  
pDeletePrev dstPtr  // 删除指针dstPtr指向的结点的前驱指针  
### 结点操作
nNew newPtr, value    // 新建值为value的结点，由newPtr指向  
nSetValue dstPtr, value // 将dstPtr指向的结点值设置为value  
nDelete srcPtr    // 删除srcPtr指向的结点,务必保证所有指向srcPtr结点的前驱后继指针均已断开连接  
nMoveAbs dstPtr, CanvasLeft, CanvasTop  // 将dstPtr指向的结点移动至绝对位置(CanvasLeft, CanvasTop)  
nMoveRel dstPtr, srcPtr, CanvasLeft, CanvasTop  // 将dstPtr指向的结点移动至srcPtr相对位置(CanvasLeft, CanvasTop)  

### 动画操作
aLeft srcPtr, offset    // 将srcPtr指向的结点及其后继向左移动  
aStd     // 将整个链表移动到标准位置  
aLine value     // 将代码区的滑块移动到value行

### 标量操作
sInit scalar value  // 设置标量变量初始值    
sMove dstSca, srcSca    // 将srcSca值赋给dstSca  
sInc dstSca, value   // dstSca += value  
### 程序控制
gBeq ptr1, ptr2, label  // 当ptr1与ptr2指向同一个结点时跳转到PC = PC + label  
gBne ptr1, ptr2, label  // 当ptr1与ptr2指向不同结点时跳转到PC = PC + label  
sBge scalar, value, label   // 当scalar值不小于value时跳转到PC = PC + label  
vBeq genPtr, value, label   // 当genPtr指向结点值等于value时跳转到PC = PC + label  
Jmp label   // 无条件跳转至 PC + label  
vBge ptr1, ptr2, label  // 当ptr1指向的结点值大于等于ptr2指向的结点值时跳转到PC = PC + label  
vBle ptr1, ptr2, label  // 当ptr1指向的结点值小于等于ptr2指向的结点值时跳转到PC = PC + label  
Halt        // 程序结束  
Exception cause   // 结果异常, cause为原因  
Yield scalar    // 将scalar值作为执行结果进行展示  