using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Notes.Data.Abstract;
using Notes.Dtos;
using Notes.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesRepository _repo;
        private readonly IMapper _mapper;

        public NotesController(INotesRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Add(NoteForAddingDto noteDto)
        {
            var noteToAdd = _mapper.Map<Note>(noteDto);
            noteToAdd.OriginalNoteId = _repo.GetHighestOriginalNoteId() + 1;
            _repo.Add(noteToAdd);

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetLatestById(int id)
        {
            var note = _repo.GetLatestById(id);
            var noteToReturn = _mapper.Map<NoteForGettingDto>(note);

            return Ok(noteToReturn);
        }

        [HttpGet("/history/{id}")]
        public async Task<IActionResult> GetHistoryById(int id)
        {
            var notes = await _repo.GetHistoryById(id);
            var notesToRetun = _mapper.Map<IEnumerable<Note>>(notes);

            return Ok(notesToRetun);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NoteForUpdatingDto noteDto)
        {
            var noteToUpdate = await _repo.GetById<Note>(id);
            noteToUpdate.Modified = DateTime.Now;
            _repo.Update(noteToUpdate);

            var noteToAdd = _mapper.Map<Note>(noteDto);
            noteToAdd.Version = _repo.GetLatestById(id).Version + 1;
            _repo.Add(noteToAdd);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var noteToDelete = await _repo.GetById<Note>(id);

            if(noteToDelete.Deleted == true)
            {
                return BadRequest("This note doesn't exist.");
            }

            noteToDelete.Deleted = true;
            _repo.Update(noteToDelete);

            return Ok();
        }
    }
}
