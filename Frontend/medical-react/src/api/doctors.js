export async function getDoctors(filters = {}) {
  const queryParams = new URLSearchParams();

  Object.keys(filters).forEach(key => {
    if (filters[key]) {
      queryParams.append(key, filters[key]);
    }
  });

  for (const key in filters) {
    if (filters[key] !== "" && filters[key] !== null && filters[key] !== undefined) {
      queryParams.append(key, filters[key]);
    }
  }
  
  const url = `https://localhost:7260/api/profiles/doctor?${queryParams.toString()}`;
  
  const response = await fetch(url);
  if (!response.ok) throw new Error("Ошибка загрузки");
  return await response.json();
}