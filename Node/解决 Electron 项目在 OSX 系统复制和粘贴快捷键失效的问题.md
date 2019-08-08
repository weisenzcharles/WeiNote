公司的一个项目需要开发跨平台，由于整个项目完全由我一个人开发，初次接触 Electron，开发过程中遇到了不少坑，同样的代码 Windows 下复制和粘贴没有问题，Mac 下复制和粘贴失效，在网上搜了一下都是菜单栏的复制和粘贴。

相关的文章：https://www.jianshu.com/p/65eccd2b62f5

只好自己去 Electron Api 中找，随手一搜还真搜到了一个，不知道能否解决问题。

![Api](../pics/20181126150759018.png)

contents 对象是 webContents，webContents 可以通过 Window.webContents 获取到。

![Api](../pics/20181126151508174.png)

强制把复制和粘贴绑定到对应的快捷键：
```JavaScript
  if (process.platform === "darwin") {
    let contents = mainWindow.webContents;
    globalShortcut.register("CommandOrControl+C", () => {
      contents.copy();
    });
    globalShortcut.register("CommandOrControl+V", () => {
      contents.paste();
    });
  }
```

因为 Windows 平台下可以正常使用，所以我只针对了 OSX 执行此操作。

运行项目后测试这个方法确实可行。
![项目截图](../pics/20181126145718259.png)