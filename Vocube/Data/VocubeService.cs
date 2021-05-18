using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Vocube.Data {
    public class VocubeService {
        public VocubeState CurrentState { get { return vocubeState; } }

        private VocubeState vocubeState = VocubeState.WAITING_FOR_HOST;

        public delegate void StateChangeHandler();

        public event StateChangeHandler StateChange;

        public event StateChangeHandler AnswerStateChange;

        public Dictionary<string, ConcurrentBag<string>> Answers { get { return answers; } }

        private Dictionary<string, ConcurrentBag<string>> answers;

        private List<string> categories;

        public string Question { get { return question; } }

        private string question = "";

        private readonly Random random = new();

        public void NewGame(string question, List<string> categories) {
            this.question = question;
            this.categories = categories;
            this.answers = new Dictionary<string, ConcurrentBag<string>>(
                categories.ConvertAll((category) => {
                    return new KeyValuePair<string, ConcurrentBag<string>>(category, new ConcurrentBag<string>());
                })
            );
            vocubeState = VocubeState.FILLING_IN_CUBE;
            StateChange();
        }

        public void EndGame() {
            this.question = "";
            this.categories = new List<string>();
            this.answers = new Dictionary<string, ConcurrentBag<string>>();
            vocubeState = VocubeState.WAITING_FOR_HOST;
            StateChange();
            AnswerStateChange();
        }

        public string GetRandomCategory() {
            if (categories == null || categories.Count == 0) {
                return "";
            }
            return categories[random.Next(categories.Count)];
        }

        public void AddToCategory(string category, string entry) {
            answers[category].Add(entry);
            AnswerStateChange();
        }
    }

    public enum VocubeState {
        WAITING_FOR_HOST,
        FILLING_IN_CUBE
    }
}
