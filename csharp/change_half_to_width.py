import os
from os import path

SOURCE_PATH = os.getcwd() + "/2019_01_05_managed_heap_and_gc_principle_part_1.md"
TEST_PATH = os.getcwd() + "/test.md"

full2half_table = [(chr(i), chr(i - 65248)) for i in range(65281, 65374)]
full2half_table.append((chr(12288), chr(32)))

def read_all_content(p):
	f = open(p,encoding='utf-8')  
	listContent = f.readlines()
	f.close()

	return ''.join(listContent)

def write_content_to_file(p,content):
	f = open(p,'w',encoding='utf-8')
	f.write(content)
	f.close()

def main():
	#print('目前系统的编码为：',sys.getdefaultencoding())

	file_content = read_all_content(SOURCE_PATH)
	file_content = file_content.replace(',','，');
	#print(file_content)
	#binaryUnicode = file_content.encode("utf-8")

	# utf-8===>unicode
	#file_content = file_content.decode()
	#print(file_content)
	#finalContent = strB2Q(binaryUnicode)
	
	#file_content = file_content.deConnectionAbortedError()
	#print(file_content)
	#print(file_content)

	#file_content = file_content.encode('unicode_escape')
	#file_content = file_content.decode('utf-8')
	#print(finalContent)

	write_content_to_file(SOURCE_PATH,file_content)
	# utf-8===>unicode
	#b_str = bytes(file_content,encoding='unicode_escape')

	

	#print(b_str)
	
if __name__ == '__main__':
	main()

