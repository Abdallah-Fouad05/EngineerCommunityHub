using DAL;
using DAL.DTOs;

namespace BAL
{
    public class ProjectBusiness
    {
        public static async Task<List<ProjectDTO>> GetAllProjects()
        {
            try
            {
                return await ProjectData.GetAllProjects();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching projects", ex);
            }
        }

        public static async Task<List<ProjectDTO>> GetAllProjectsByUserId(int UserId)
        {
            try
            {
                return await ProjectData.GetAllProjectsByUserId(UserId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching projects", ex);
            }
        }

        public static async Task<List<ProjectDTO>> GetProjectsBySubTeamId(int id)
        {
            try
            {
                return await ProjectData.GetProjectsBySubTeamId(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching projects", ex);
            }
        }

        public static async Task<List<ProjectDTO>> GetProjectsByStatusId(int id)
        {
            try
            {
                return await ProjectData.GetProjectsByStatusId(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching projects", ex);
            }
        }


        public static async Task<ProjectDTO?> GetProjectById(int id)
        {
            try
            {
                return await ProjectData.GetProjectById(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching project", ex);
            }
        }

        public static async Task<(bool, int)> CreateProject(Project_Created p)
        {
            try
            {
                return await ProjectData.CreateProject(p);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating project", ex);
            }
        }

        public static async Task<int> UpdateProject(Project_Updated p)
        {
            try
            {
                return await ProjectData.UpdateProject(p);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error updating project", ex);
            }
        }

        public static async Task<int> DeleteProject(int id)
        {
            try
            {
                return await ProjectData.DeleteProject(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error deleting project", ex);
            }
        }

        public static async Task<List<StatusDTO>> GetProjectStatus()
        {
            try
            {
                return await ProjectData.GetProjectStatus();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all Status.", ex);
            }
        }

        //projectMember

        public static async Task<List<ProjectMemberDTO>> GetProjectMembers(int ProjectId)
        {
            try
            {
                return await ProjectData.GetProjectMembers(ProjectId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching project members.", ex);
            }
        }

        public static async Task<List<MemberProjects>> GetProjectsByMemberId(int MemberId)
        {
            try
            {
                return await ProjectData.GetProjectsByMemberId((MemberId));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching project members.", ex);
            }
        }
        public static async Task<(bool success, int result)> AddProjectMember(ProjectMember_Created m)
        {
            try
            {
                return await ProjectData.AddProjectMember(m);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding project member.", ex);
            }
        }

        public static async Task<int> RemoveProjectMember(int projectId, int userId)
        {
            try
            {
                return await ProjectData.RemoveProjectMember(projectId, userId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error removing project member.", ex);
            }
        }
    }
}