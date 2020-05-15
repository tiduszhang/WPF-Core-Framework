多国语言配置参考说明：
 多国语言文件配置方法，一个语言一个配置文件，默认存放位置为程序目录下“lang”文件夹内，使用xml文件结构，编码格式使用UTF-8。
 <langs>
     <config Id="【语言唯一标识，可用代码替代，如：zh_CN】" name="【语言名称】" code="【语言编号】"/>
     <lang key="【关键字1】" value="【值】" name="[显示名称]" description="[描述]" prompt="[水印]" shortname="[短名称（如果没有设置则返回key的值）]" error="[错误提示]"/>
     <lang key="【关键字2】" value="【值】"/>
 </langs>