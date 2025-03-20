# ms-graph-sp
microsoft graph api for sharepoint
>
>   1.读取apilist.xml文件获取API地址列表List-A  
>    2.读取postdata.xml获取数据列表List-B  
>    3.读取存储结果的文件result.xml  
>    4.从List-A获取单个API地址信息Link-A及http请求方式  
>    &nbsp;&nbsp;&nbsp;&nbsp;*如果是get请求则进入第5步，如果是post请求则进入第9步*
>  
>    5.使用Http get访问单个API地址Link-A获取响应数据 Data-A  
>    6.获取对应文档中的响应数据 Data-B  
>    7.对比Data-A和Data-B的结构获得对比结果 Result并显示到控制台  
>    8.将Result写入result.xml  
>        
>    9.从List-B中获取Link-A对应的Postdata-A  
>    10.使用http post将数据Postdata-A post到Link-A获取响应数据 Data-C  
>    11.获取对应文档中的响应数据 Data-D  
>    12.对比Data-C和Data-D的结构获得对比结果 Result并显示到控制台  
>    13.将Result写入result.xml  
>       
>    14.从第3步开始重复，直至List-A读取完毕  
>    15.读取result.xml，统计对比结果并显示到控制台  
