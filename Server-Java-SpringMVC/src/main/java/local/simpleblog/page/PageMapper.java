package local.simpleblog.page;

import java.util.List;

import org.apache.ibatis.annotations.Mapper;
import org.apache.ibatis.annotations.Select;
import org.apache.ibatis.annotations.Insert;
import org.apache.ibatis.annotations.Update;
import org.apache.ibatis.annotations.Delete;
import org.apache.ibatis.annotations.Param;

@Mapper
public interface PageMapper {

  @Select("SELECT * FROM page")
  List<Page> getAll();

  @Select("SELECT * FROM page where id = #{id}")
  Page getById(@Param("id") int id);

  @Select("SELECT * FROM page where url = #{url}")
  Page getByUrl(String url);

  @Insert("INSERT INTO page (user_id,url,title,content,date_published,date_modified) VALUES (#{user_id},#{url},#{title},#{content},#{date_published},#{date_modified})")
  int add(Page page);

  @Update("UPDATE page SET user_id=#{user_id},url=#{url},title=#{title},content=#{content},date_published=#{date_published},date_modified=#{date_modified} WHERE id=#{id}")
  int updateById(Page page);

  @Delete("DELETE FROM page WHERE id=#{id}")
  int deleteById(int id);

}