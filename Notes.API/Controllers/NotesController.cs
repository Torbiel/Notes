using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Notes.Data.Abstract;
using Notes.Dtos;
using Notes.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Notes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesRepository _repo;
        private readonly IMapper _mapper;
        private string nonexistentNoteMessage = "This note doesn't exist.";

        public NotesController(INotesRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Add(NoteForAddingDto noteDto)
        {
            var noteToAdd = _mapper.Map<Note>(noteDto);
            _repo.Add(noteToAdd);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLatestById(int id)
        {
            var note = await _repo.GetLatestById(id);
            
            if(note == null)
            {
                return BadRequest(nonexistentNoteMessage);
            }

            var noteToReturn = _mapper.Map<NoteForGettingDto>(note);

            return Ok(noteToReturn);
        }

        [HttpGet("history/{id}")]
        public async Task<IActionResult> GetHistoryById(int id)
        {
            var notes = await _repo.GetHistoryById(id);

            if (notes == null)
            {
                return BadRequest(nonexistentNoteMessage);
            }

            var notesToRetun = _mapper.Map<IEnumerable<Note>>(notes);

            return Ok(notesToRetun);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NoteForUpdatingDto noteDto)
        {
            var noteToUpdate = await _repo.GetById<Note>(id);

            if(noteToUpdate == null || noteToUpdate.Deleted == true)
            {
                return BadRequest(nonexistentNoteMessage);
            }

            // If this is the first time updating a note, we fill its OriginalNoteId with its own Id.
            // Doing this when adding would require to make another call to db to retrieve it and another to save it again.
            if(noteToUpdate.OriginalNoteId == 0)
            {
                noteToUpdate.OriginalNoteId = noteToUpdate.Id;
            }

            noteToUpdate.Modified = DateTime.Now;
            _repo.Update(noteToUpdate);

            var noteToAdd = _mapper.Map<Note>(noteDto);
            var latestVersion = await _repo.GetLatestById(id);
            noteToAdd.Version = latestVersion.Version + 1;
            noteToAdd.OriginalNoteId = noteToUpdate.OriginalNoteId;
            _repo.Add(noteToAdd);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var noteToDelete = await _repo.GetById<Note>(id);

            if(noteToDelete == null || noteToDelete.Deleted == true)
            {
                return BadRequest(nonexistentNoteMessage);
            }

            noteToDelete.Deleted = true;
            _repo.Update(noteToDelete);

            return Ok();
        }
    }
}
