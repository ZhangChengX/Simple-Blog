package local.simpleblog.user;

import java.util.List;

import org.apache.ibatis.annotations.Mapper;
import org.apache.ibatis.annotations.Select;
import org.apache.ibatis.annotations.Insert;
import org.apache.ibatis.annotations.Update;
import org.apache.ibatis.annotations.Delete;
import org.apache.ibatis.annotations.Param;

@Mapper
public interface UserMapper {
    
    @Select("SELECT * FROM user")
    List<User> getAll();

    @Select("SELECT * FROM user where id = #{id}")
    User getById(@Param("id") int id);

    @Select("SELECT * FROM user where username = #{username}")
    User getByUsername(@Param("username") String username);

    @Insert("INSERT INTO user (username,password) VALUES (#{username},#{password})")
    int add(User user);

    @Update("UPDATE user SET username=#{username},password=#{password} WHERE id=#{id}")
    int updateById(User user);

    @Delete("DELETE FROM user WHERE id=#{id}")
    int deleteById(int id);
    
}
